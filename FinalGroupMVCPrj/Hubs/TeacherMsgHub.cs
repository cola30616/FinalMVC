using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace FinalGroupMVCPrj.Hubs
{
    public class TeacherMsgHub :Hub
    {
        private readonly LifeShareLearnContext _context;
        public TeacherMsgHub(LifeShareLearnContext context)
        {
            _context = context;
        }
        // 用戶連線 ID 列表
        public static List<string> ConList = new List<string>();

        public static List<TChatMessageTeacher> MsgList =new List<TChatMessageTeacher>();
        
        public static Dictionary<string, string> teacherDict = new Dictionary<string, string>();

        public static Dictionary<string, string> studentDict = new Dictionary<string, string>();

        public static Dictionary<string, string> PushStudentDict = new Dictionary<string, string>();

        public static List<TMemberGetPush> GetPushList = new List<TMemberGetPush>();

        public static Dictionary<string, string> adminDict = new Dictionary<string, string>();

        public static List<TPushMessage> adminMsgList = new List<TPushMessage>();


        /// 連線事件
        public override async Task OnConnectedAsync()
        {
            if (ConList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            {
                ConList.Add(Context.ConnectionId);
            }
            //測試訊息
            //TChatMessageTeacher msg1 = new TChatMessageTeacher();
            //msg1.FMessageId = 1;
            //msg1.FTeacherId = 2;
            //msg1.FMemberId = 1;
            //msg1.FMessage = "測試測試";
            //msg1.FMessageTime = DateTime.Now;
            //msg1.FIsTeacherMsg = true;

            //MsgList.Add(msg1);
            //TChatMessageTeacher msg2 = new TChatMessageTeacher();
            //msg2.FMessageId = 2;
            //msg2.FTeacherId = 2;
            //msg2.FMemberId = 1;
            //msg2.FMessage = "測試測試回復";
            //msg2.FMessageTime = DateTime.Now;
            //msg2.FIsTeacherMsg = false;
            //MsgList.Add(msg2);

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新個人 ID
            //await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

            // 更新聊天內容
            //await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task SendTeacherId(string teacherId)
        {
            teacherDict[Context.ConnectionId] = teacherId;

            await Clients.Caller.SendAsync("ConnectionEstablished", "Connection established successfully!");
        }

        public async Task SendStudentId(string memberId)
        {
            studentDict[Context.ConnectionId] = memberId;

            await Clients.Caller.SendAsync("ConnectionEstablished", "Connection established successfully!");
        }

        public async Task SendPushStudentId(string memberId)
        {
            PushStudentDict[Context.ConnectionId] = memberId;

            await Clients.Caller.SendAsync("ConnectionEstablished", "Connection established successfully!");
        }


        public async Task SendAdminId(string adminId)
        {
            adminDict[Context.ConnectionId] = adminId;

            await Clients.Caller.SendAsync("ConnectionEstablished", "Connection established successfully!");
        }

        public async Task<List<TChatMessageTeacher>> GetChatMessages(string teacherId,string memberId)
        {
            var messages = MsgList.Where(msg => msg.FTeacherId == Convert.ToInt32(teacherId) && msg.FMemberId == Convert.ToInt32(memberId)).ToList();
            return messages;
        }
        public async Task<List<TChatMessageTeacher>> GetStudentChatMessages(string studentId)
        {
            var messages = MsgList.Where(msg => msg.FMemberId == Convert.ToInt32(studentId)).ToList();
            return messages;
        }
        public async Task<List<string>> GetConnectionList()
        {
            var messages = studentDict.Values.ToList();
            return messages;
        }

        public async Task<bool> MemberIsOnline(string memberId)
        {
            var isOnline = studentDict.ContainsValue(memberId);
            return isOnline;
        }

        public async Task<bool> TeacherIsOnline(string teacherId)
        {
            var isOnline =  teacherDict.ContainsValue(teacherId);
            return isOnline;
        }

        /// 離線事件
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string id = ConList.Where(p => p == Context.ConnectionId).FirstOrDefault();
            if (id != null)
            {
                ConList.Remove(id);
            }

            if (studentDict.ContainsKey(Context.ConnectionId))
            {
                studentDict.Remove(Context.ConnectionId);
            }
            else if(teacherDict.ContainsKey(Context.ConnectionId))
            {
                teacherDict.Remove(Context.ConnectionId);
            }
            else if (PushStudentDict.ContainsKey(Context.ConnectionId))
            {
                PushStudentDict.Remove(Context.ConnectionId);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConList);
            await Clients.All.SendAsync("UpdList", jsonString);

            //// 更新聊天內容
            //await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);

            //await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SendMessage(string teacherId,  string memberId, string message, DateTime date, bool isteacher)
        {
            TChatMessageTeacher msg = new TChatMessageTeacher();

            msg.FTeacherId = Convert.ToInt32(teacherId);
            msg.FMemberId = Convert.ToInt32(memberId);
            msg.FMessage = message;
            msg.FMessageTime = date;
            msg.FIsTeacherMsg = isteacher;
            MsgList.Add(msg);

            _context.Add(msg);
            _context.SaveChanges();

                // 接收人
                var queryStudent = studentDict.Where(key => key.Value == memberId);
                //await Clients.Client(memberId).SendAsync("UpdContent",msg);
                if(queryStudent.Any())
                {
                    foreach (var item in queryStudent)
                    {
                        await Clients.Client(item.Key).SendAsync("UpdContent", msg, teacherId, memberId);
                    }
                }
                // 發送人
                var queryTeacher = teacherDict.Where(key => key.Value == teacherId);
                if(queryTeacher.Any())
                {
                    foreach (var item in queryTeacher)
                    {
                        await Clients.Client(item.Key).SendAsync("UpdContent", msg,teacherId,memberId);
                    }
                }
        }

        public async Task SendPushMsg(List<int> selectedMembers, string pushDelay)
        {
            int delaySeconds = Convert.ToInt32(pushDelay);

            
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

            
            foreach (int memberId in selectedMembers)
            {
               
                if (PushStudentDict.ContainsValue(memberId.ToString()))
                {
                    
                    var connectionId = PushStudentDict.FirstOrDefault(x => x.Value == memberId.ToString()).Key;

                    if (connectionId != null)
                    {
                        await Clients.Client(connectionId).SendAsync("UploadNF");
                    }
                }
            }
            // 發送人
            //var queryTeacher = teacherDict.Where(key => key.Value == teacherId);
            //if (queryTeacher.Any())
            //{
            //    foreach (var item in queryTeacher)
            //    {
            //        await Clients.Client(item.Key).SendAsync("UpdContent", msg, teacherId, memberId);
            //    }
            //}
        }



    }
}
