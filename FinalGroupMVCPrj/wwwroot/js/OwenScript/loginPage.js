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