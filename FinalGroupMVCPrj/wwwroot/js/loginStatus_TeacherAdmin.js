document.addEventListener('DOMContentLoaded', function () {
    // 在這裡放置當文檔準備好後要執行的 JavaScript 代碼
    // 相當於 $(document).ready() 的功能
    let teacherIdString = '0';
    let teacherId = 0;
    const teacherPic = document.getElementById('teacherPic');
    const navTeacherName = document.getElementById('navTeacherName');
    const teacherName = document.getElementById('teacherName');

    const adjustLayout = async () => {
        try {
            const response = await fetch('https://localhost:7031/UserInfo/APICurrentTeacherId', {
                method: "GET",
            });
            teacherIdString = await response.text();
            console.log(teacherIdString);
            if (teacherIdString !== '0') {
                teacherId = Number(teacherIdString);
                changeTeacherInfo();
                changeTeacherPhoto();

            }


        } catch (error) {
            console.error('Error fetching teacher ID:', error);
        }
    };

    const changeTeacherPhoto = async () => {
        try {
            const response = await fetch(`https://localhost:7031/Teacher/TeacherPhoto/${teacherId}`, {
                method: "GET",
            });
            let photoUrl = '';
            photoUrl = await response.text();

            if (photoUrl !== '') {
                teacherPic.src = photoUrl;
            }
        } catch (error) {
            console.error('Error fetching teacher Photo:', error);
        }
    };

    const changeTeacherInfo = async () => {
        try {
            const response = await fetch(`https://localhost:7031/UserInfo/APITeacherInfo?teacherId=${teacherId}`, {
                method: "GET",
            });
            let teacherInfo = await response.json();
            navTeacherName.innerHTML = teacherInfo.TeacherName;
            teacherName.innerHTML = teacherInfo.TeacherName;
        } catch (error) {
            console.error('Error fetching teacherInfo:', error);
        }
    };

    adjustLayout();
});
