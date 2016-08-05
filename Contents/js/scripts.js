$(document).ready(function(){

$(".editStylistForm form").hide();
  $(".editStylistButton").click(function(){
    $(this).next().show();
  });


$(".editNameInput").change(inputEvent).keyup(inputEvent);

function inputEvent(){
  var temp=$(this).val();
  $(this).parent().parent().find("span.editNameInputDisplay").text(temp);
}
});
