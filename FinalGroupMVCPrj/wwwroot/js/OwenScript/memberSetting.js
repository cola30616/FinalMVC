document.addEventListener('DOMContentLoaded', function () {
    const inputMemPic = document.getElementById("inputMemPic");
    const changeMemPic =   document.getElementById("changeMemPic")
    new lc_select(document.querySelector('.liveCityLcSelect'), {
        // options here
        max_opts: 3,
        addit_classes: ['lcslt-light']
    });
    new lc_select(document.querySelector('.wishFieldLcSelect'), {
        // options here
        max_opts: 3,
        addit_classes: ['lcslt-light']
    });
    inputMemPic.addEventListener('change', () => {
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
            const result = updateImage(file);
            if (result === false) { return; }

            reader.onload = function () {
                if (reader.readyState == 2) {
                    document.querySelectorAll(".memberPic").forEach((e) => { e.src = reader.result;})
                }
            }
        } 
    }
    async function updateImage(file) {
        let formData = new FormData();
        formData.append('file',file);
        const response = await fetch(`https://localhost:7031/Member/UpdatePhoto`, {
            method: 'POST',
            body: formData
        });
        if (response.ok) {
            Swal.fire({
                position: "center",
                icon: "success",
                title: "變更頭像成功",
                showConfirmButton: false,
                timer: 1500
            });
            return true;
        } else {
            Swal.fire({
                position: "center",
                icon: "error",
                title: "變更頭像失敗",
                showConfirmButton: false,
                timer: 1500
            });
            return false;
        }
       
    }
});