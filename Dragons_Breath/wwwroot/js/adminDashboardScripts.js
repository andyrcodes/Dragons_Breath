function editTicketSettings(labelId, inputId1, inputId2) {
    var input1 = document.getElementById(inputId1);
    var input2 = document.getElementById(inputId2);
    var label1 = document.getElementById(labelId);
    if (input1.style.display == "none") {
        input1.style.display = "block";
    };
    if (input2.style.display == "none") {
        input2.style.display = "block";
    };
    if (label1.style.display == "none") {
        label1.style.display = "block";
    };
}

function editUser(userId) {
    var show1 = "fN-" + userId;
    var show2 = "lN-" + userId;
    var show3 = "email-" + userId;
    var show4 = "role-" + userId;
    var show5 = "editBtn-" + userId;

    var hide1 = "editFN-" + userId;
    var hide2 = "editLN-" + userId;
    var hide3 = "editEmail-" + userId;
    var hide4 = "editRole-" + userId;
    var hide5 = "submitBtn-" + userId;

    var label1 = document.getElementById(show1);
    var label2 = document.getElementById(show2);
    var label3 = document.getElementById(show3);
    var label4 = document.getElementById(show4);
    var label5 = document.getElementById(show5);

    var input1 = document.getElementById(hide1);
    var input2 = document.getElementById(hide2);
    var input3 = document.getElementById(hide3);
    var input4 = document.getElementById(hide4);
    var input5 = document.getElementById(hide5);

    if (label1.style.display == "block") {
        label1.style.display = "none";
    };
    if (label2.style.display == "block") {
        label2.style.display = "none";
    };
    if (label3.style.display == "block") {
        label3.style.display = "none";
    };
    if (label4.style.display == "block") {
        label4.style.display = "none";
    };
    if (label5.style.display == "block") {
        label5.style.display = "none";
    };

    if (input1.style.display == "none") {
        input1.style.display = "block";
    };
    if (input2.style.display == "none") {
        input2.style.display = "block";
    };
    if (input3.style.display == "none") {
        input3.style.display = "block";
    };
    if (input4.style.display == "none") {
        input4.style.display = "block";
    };
    if (input5.style.display == "none") {
        input5.style.display = "block";
    };
}

function editFilterUser(userId) {

}

$(function () {
    //Initialize Select2 Elements
    $('.select2').select2()

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })
    //Bootstrap Duallistbox
    $('.duallistbox').bootstrapDualListbox()
})

$(function () {
    $('.table').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});