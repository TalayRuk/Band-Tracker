$(document).ready(function(){

$("form.editButtonShow").addClass("displayNone");
  $(".editButton").click(function(){
    $(this).next().toggleClass("displayNone");
  });


$(".editNameInput").change(inputEvent).keyup(inputEvent);

function inputEvent(){
  var temp=$(this).val();
  $(this).parent().parent().find("span.editNameInputDisplay").text(temp);
}
});
