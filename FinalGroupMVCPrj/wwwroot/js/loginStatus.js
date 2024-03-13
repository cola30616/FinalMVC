document.addEventListener('DOMContentLoaded', function () {
    // 在這裡放置當文檔準備好後要執行的 JavaScript 代碼
    // 相當於 $(document).ready() 的功能
    let memberIdString = '0';
    let memberId = 0;
    const toLogin = document.getElementById('toLogin');
    const isLogin = document.getElementById('isLogin');
    const memberPic = document.querySelectorAll('.memberPic');
    const navShowName = document.getElementById('navShowName');
    const userShowName = document.getElementById('userShowName');
    const userEmail = document.getElementById('userEmail');

    const adjustLayout = async () => {
        try {
            const response = await fetch('https://localhost:7031/UserInfo/APICurrentMemberId', {
                method: "GET",
            });
            memberIdString = await response.text();
            console.log(memberIdString);
            if (memberIdString !== '0') {
                toLogin.style.display = 'none';
                isLogin.style.display = 'block';
                memberId = Number(memberIdString);
                changeMemberInfo();
                changeMemberPhoto();
                checkRole();
            }


        } catch (error) {
            console.error('Error fetching member ID:', error);
        }
    };

    const changeMemberPhoto = async () => {
        try {
            const response = await fetch(`https://localhost:7031/Member/MemberPhoto/${memberId}`, {
                method: "GET",
            });
            let photoUrl = '';
            photoUrl = await response.text();

            if (photoUrl !== '') {
                memberPic.forEach((e) => { e.src = photoUrl; })
            }
        } catch (error) {
            console.error('Error fetching member Photo:', error);
        }
    };

    const changeMemberInfo = async () => {
        try {
            const response = await fetch(`https://localhost:7031/UserInfo/APICurrentMemberInfo/${memberId}`, {
                method: "GET",
            });
            let memberInfo = await response.json();
            navShowName.innerHTML = memberInfo.ShowName;
            userShowName.innerHTML = memberInfo.ShowName;
            userEmail.innerHTML = memberInfo.Email;
        } catch (error) {
            console.error('Error fetching memberInfo:', error);
        }
    };
    const checkRole = async () => {
        try {
            const response = await fetch(`https://localhost:7031/UserInfo/APICurrentTeacherId`, {
                method: "GET",
            });
            let teacherIdString = await response.text();
            if (teacherIdString !== '0') {
                document.querySelectorAll('.toggleTAdmin').forEach(element => {
                    element.style.display = 'block';
                });
            }
        } catch (error) {
            console.error('Error fetching memberInfo:', error);
        }
    };


    adjustLayout();
});
