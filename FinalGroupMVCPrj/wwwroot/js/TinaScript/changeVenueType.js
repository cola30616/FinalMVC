//根據場地變換詳細場地資訊的顯示
const Venuetype = document.getElementsByName("VenueType");
const isOnline = document.querySelector("#onClassInfo");
// console.log(isOnline);
for (let i = 0; i < Venuetype.length; i++) {
    Venuetype[i].addEventListener("change", function () {
        if (Venuetype[i].id == 'online') {
            isOnline.hidden = true;
        }
        else {
            isOnline.hidden = false;
        }
    });
}