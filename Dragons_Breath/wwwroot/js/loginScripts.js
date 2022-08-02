
$("#demoAdminBtn").click(function () {
    var target = '<a href="javascript:document.getElementById(\'demoAdmin\').submit()" class="text-light" >Got It!</a>';
    Swal.fire({
        title: 'Demo Admin Login',
        text: "Demo users have very limited database access",
        icon: 'info',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: target,
    })
})
$("#demoPMBtn").click(function () {
    var target = '<a href="javascript:document.getElementById(\'demoPM\').submit()" class="text-light" >Got It!</a>';
    Swal.fire({
        title: 'Demo Project Manager Login',
        text: "Demo users have very limited database access",
        icon: 'info',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: target,
    })
})
$("#demoDevBtn").click(function () {
    var target = '<a href="javascript:document.getElementById(\'demoDev\').submit()" class="text-light" >Got It!</a>';
    Swal.fire({
        title: 'Demo Developer Login',
        text: "Demo users have very limited database access",
        icon: 'info',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: target,
    })
})
$("#demoSubBtn").click(function () {
    var target = '<a href="javascript:document.getElementById(\'demoSub\').submit()" class="text-light" >Got It!</a>';
    Swal.fire({
        title: 'Demo Submitter Login',
        text: "Demo users have very limited database access",
        icon: 'info',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: target,
    })
})

