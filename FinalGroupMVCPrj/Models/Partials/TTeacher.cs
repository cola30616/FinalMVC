using FinalGroupMVCPrj.Models.Metadatas;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Models
{
    //描述TVenue類別的Metadata類別叫做VenueMetadata
    [ModelMetadataType(typeof(TTeacherMatadata))]
    public partial class TTeacher
    {       
        //已經不能做文章(因為TVenue類別)
        //因此定一個類別來描述我
    }
}
