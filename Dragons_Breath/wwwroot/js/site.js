$('.summernote').summernote({
    tabsize: 2,
    height: 210,
    toolbar: [
        // [groupName, [list of button]]
        ['style', ['bold', 'italic', 'underline', 'clear']],
        ['font', ['strikethrough', 'superscript', 'subscript']],
        ['fontsize', ['fontsize']],
        ['color', ['color']],
        ['para', ['ul', 'ol', 'paragraph']],
        ['height', ['height']]
    ]
});

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

document.getElementById('editAll').addEventListener('click', editAll);
document.getElementById('titleProjectEdit').addEventListener('click', titleProjectEdit);
document.getElementById('descriptionEdit').addEventListener('click', descriptionEdit);
document.getElementById('priorityEdit').addEventListener('click', priorityEdit);
document.getElementById('typeEdit').addEventListener('click', typeEdit);
document.getElementById('statusEdit').addEventListener('click', statusEdit);
document.getElementById('developerEdit').addEventListener('click', developerEdit);
document.getElementById('editComment').addEventListener('click', editComment);


function editAll() {
    titleProjectEdit();
    descriptionEdit();
    priorityEdit();
    statusEdit();
    typeEdit();
    developerEdit();
    var button = document.getElementById('editAll');
    button.style.display = "none";

}

function titleProjectEdit() {
    var input1 = document.getElementById('titleInput');
    var input2 = document.getElementById('projectInput');
    var label1 = document.getElementById('titleLabel');
    var label2 = document.getElementById('projectLabel');
    var footer = document.getElementById('footer');
    var button = document.getElementById('titleProjectEdit');
    if (input1.style.display == "none") {
        input1.style.display = "block";
        button.style.display = "none";
    };
    if (input2.style.display == "none") {
        input2.style.display = "block";
        input2.classList.add('select2bs4');
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })
    };
    if (label1.style.display == "none") {
        label1.style.display = "block";
    };
    if (label2.style.display == "none") {
        label2.style.display = "block";
    };
    if (footer.style.display == "none") {
        footer.style.display = "block";
    };
}

function descriptionEdit() {
    var input = document.getElementById('ticketDescription');
    var footer = document.getElementById('footer');
    var button = document.getElementById('descriptionEdit');
    if (input.style.display == "none") {
        input.style.display = "block";
        button.style.display = "none";
        input.classList.add('summernote');
        $('.summernote').summernote({
            tabsize: 2,
            height: 200,
            toolbar: [
                // [groupName, [list of button]]
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['font', ['strikethrough', 'superscript', 'subscript']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']]
            ]
        });

    };
    if (footer.style.display == "none") {
        footer.style.display = "block";
    };
}

function priorityEdit() {
    var input = document.getElementById('ticketPriority');
    var footer = document.getElementById('footer');
    if (input.style.display == "none") {
        input.style.display = "block";
        input.classList.add('select2bs4');
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

    };
    if (footer.style.display == "none") {
        footer.style.display = "block";
    };
}

function statusEdit() {
    var input = document.getElementById('ticketStatus');
    var footer = document.getElementById('footer');
    if (input.style.display == "none") {
        input.style.display = "block";
        input.classList.add('select2bs4');
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

    };
    if (footer.style.display == "none") {
        footer.style.display = "block";
    };
}

function typeEdit() {
    var input = document.getElementById('ticketType');
    var footer = document.getElementById('footer');
    if (input.style.display == "none") {
        input.style.display = "block";
        input.classList.add('select2bs4');
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

    };
    if (footer.style.display == "none") {
        footer.style.display = "block";
    };
}

function developerEdit() {
    var input = document.getElementById('developer');
    var footer = document.getElementById('footer');
    if (input.style.display == "none") {
        input.style.display = "block";
        input.classList.add('select2bs4');
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

    };
    if (footer.style.display == "none") {
        footer.style.display = "block";
    };
}

function editComment() {
    var input = document.getElementById('commentEdit');
    var button = document.getElementById('editSubmit');
    var button2 = document.getElementById('editComment');

    if (input.style.display == "none") {
        input.style.display = "block";
        button.style.display = "block";
        button2.style.display = "none";
        input.classList.add('summernote');
        $('.summernote').summernote({
            tabsize: 2,
            height: 200,
            toolbar: [
                // [groupName, [list of button]]
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['font', ['strikethrough', 'superscript', 'subscript']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']]
            ]
        });

    };
}