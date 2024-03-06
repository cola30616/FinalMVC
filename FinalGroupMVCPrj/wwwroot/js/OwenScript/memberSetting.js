document.addEventListener('DOMContentLoaded', function () {
    const inputMemPic = document.getElementById("inputMemPic");
    const changeMemPic =   document.getElementById("changeMemPic")
    new lc_select(document.querySelector('.liveCityLcSelect'), {
        // options here
        max_opts: 3,
        addit_classes: ['lcslt-light']
    });
    new lc_select(document.querySelector('.wishField'), {
        // options here
        max_opts: 3,
        addit_classes: ['lcslt-light']
    });
    inputMemPic.addEventListener('change', () => {
        console.log('hi33');
        checkImage(inputMemPic);
    })
    function checkImage(input) {
        const invalidPhotoWarn = document.getElementById("invalidPhotoWarn");
        invalidPhotoWarn.style.display = 'none';

        if (input.files) {
            let file = input.files[0];
            let reader = new FileReader();

            reader.readAsDataURL(file);
           
       let allowedImageTypes = ["image/jpeg", "image/gif", "image/png"];
            if (!allowedImageTypes.includes(file.type)) {
                invalidPhotoWarn.style.display = 'block';
                invalidPhotoWarn.innerHTML = "僅接受 .jpg .jpeg .png 圖片";
                return ;
            }
            if (file.size > 1024 * 1024 * 1) {
                invalidPhotoWarn.style.display = 'block';
                invalidPhotoWarn.innerHTML = "檔案需小於 1 MB";
                return;
            }
            reader.onload = function () {
                if (reader.readyState == 2) {
                    document.querySelectorAll(".memberPic").forEach((e) => { e.src = reader.result;})
                }
            }
        } 
    }
    function updateImage() {

    }
});