$(function () {

    // Confirm product delete
    var table = $("table#pages tbody");
    var originalTextBoxValue;
    $("a.delete").click(function () {
        if (!confirm("Confirm product deletion")) return false;
    });
    $(document).ready(function () {

        $("table#tablepage :button").click(function () {
            var input_text_id = $(this).attr("data-value");
            var input_id = "#" + input_text_id;
            if ($(this).val() == "Edit") {
                //Get input and remove Readonly

                $(input_id).attr("readonly", false);
                //Set Value to Save
                $(this).val("Save");
            } else {
                var input_text = $(input_id).val();            
               
                $(input_id).attr("readonly", true);
                $(this).val("Edit");
                $(this).load('@Url.Action("EditCategory","Shop")?name=' + input_text + '&id=' + input_text_id, function (response, status, xhr) {
                    console.log(response);
                    console.log(status);
                    if (status == "error") {
                        var msg = "Sorry but there was an error: ";
                        $("#error").html(msg + xhr.status + " " + xhr.statusText);
                    } else
                        if (response == "ok") {
                        msg = "The category name has been changed!";
                        $("#succes").html(msg);
                        $("#succes").attr("hidden", false);
                        setTimeout(function () { $("#succes").attr("hidden", true); }, 1000);
                    }
                });
            }
        });
        setTimeout(function () { $('#addCatSuccess').attr("hidden", true) }, 1000);
        setTimeout(function () { $('#addCatFail').attr("hidden", true) }, 1000);


    });
    $()
});