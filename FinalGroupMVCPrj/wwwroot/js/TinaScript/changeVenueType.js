//根據場地變換詳細場地資訊的顯示
const Venuetype = document.getElementsByName("FVenueType");
const isOnline = document.querySelector("#onClassInfo");
const online = document.querySelector("#onlineInfo");
// console.log(isOnline);
for (let i = 0; i < Venuetype.length; i++) {
    Venuetype[i].addEventListener("change", function () {
        if (Venuetype[i].id == 'online') {
            isOnline.hidden = true;
            online.hidden = false;
        }
        else {
            isOnline.hidden = false;
            online.hidden = true;
        }
    });
}