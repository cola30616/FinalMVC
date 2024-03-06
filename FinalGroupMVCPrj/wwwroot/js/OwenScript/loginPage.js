document.addEventListener('DOMContentLoaded', function () {
    const signupTogle = document.querySelectorAll(".signupTogle");
    const loginTogle = document.querySelectorAll(".loginTogle");
    const toggleToSignup = document.getElementById("toggleToSignup")
    const toggleToLogin = document.getElementById("toggleToLogin")
    const submitBtn = document.getElementById("submitBtn")
    toggleToSignup.addEventListener("click", () => {
        ToSignup();
    });

    toggleToLogin.addEventListener("click", () => {
        ToLogin();

        // 使用 Bowser 解析用户代理字符串
        //const userAgent = navigator.userAgent;
        //console.log(userAgent);
        //const platformInfo = platform.parse();

        // 获取浏览器、操作系统等信息
    //    const browserName = platformInfo.name;
    //    const browserVersion = platformInfo.version;
    //    const osName = platformInfo.os.family;
    //    const product = platformInfo.product;
    //    console.log('Browser:', browserName, 'Version:', browserVersion);
    //    console.log('Operating System:', osName);
    //    console.log(platformInfo);
    //    if ('geolocation' in navigator) {
    //        navigator.geolocation.getCurrentPosition(
    //            successCallback,
    //            errorCallback,
    //            { enableHighAccuracy: true } 
    //        );
    //    } else {
    //        console.log('不支援Geolocation API');
    //    }

    //    function successCallback(position) {
    //        const latitude = position.coords.latitude;
    //        const longitude = position.coords.longitude;

    //        //網站資訊https://nominatim.org/release-docs/develop/api/Reverse/
    //        const apiUrl = `https://nominatim.openstreetmap.org/reverse?format=json&lat=${latitude}&lon=${longitude}&zoom=12`;
    //        fetch(apiUrl)
    //            .then(response => response.json())
    //            .then(data => {
    //                console.log(data.address.suburb);
    //            })
    //            .catch(error => {
    //                console.error('錯誤：', error);
    //            });
    //    }

    //    function errorCallback(error) {
    //        // 获取位置信息失败
    //        console.error('获取位置信息失败', error.message);
    //    }
    ////取得ip資訊
    //    fetch('https://ipinfo.io/json')
    //        .then(response => response.json())
    //        .then(data => {
    //            console.log(data);
    //        })
    //        .catch(error => console.error('取得IP錯誤', error));

    });



    function ToSignup() {
        signupTogle.forEach((element) => {
            element.style.display = "block";
        });
        loginTogle.forEach((element) => {
            console.log("hi2");
            element.style.display = "none";
        });
        submitBtn.innerHTML = "註冊"
    };

    function ToLogin() {
        signupTogle.forEach((element) => {
            element.style.display = "none";
        });
        loginTogle.forEach((element) => {
            element.style.display = "block";
        });
        submitBtn.innerHTML = "登入"
    };

});