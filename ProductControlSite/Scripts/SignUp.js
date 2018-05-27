function equal_password() {
    password = $('#password').val();
    confirmpassword = $('#confirmpassword').val();
    if (password == confirmpassword) {
        $('#p_confpass').visibility = "hidden";
        return true;
    }
    $('#p_confpass').visibility = "visible";
    return false;
}
$("form").submit(function (event) {
        event.preventDefault();

    url = "http://localhost:50924/Account/Register";
    localStorage.clear();
    email_ = $('#email').val();
    firstname_=$('#firstName').val();
    secondname_=$('#secondName').val();
    password_ = $('#password').val();
    confirmpassword_ = $('#confirmpassword').val();
    fetch(url,{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({Email: email_, 
            FirstName:firstname_, 
            SecondName: secondname_, 
            Password: password_, 
            ConfirmPassword: confirmpassword_,
            Role:"user"})
    })
        .then((response) => {
            if(response.ok) {
                return response.json();
            }

            throw new Error('Network response was not ok');
        })
        .then((json) => {
            alert(json);
        })
        .catch((error) => {
            console.log(error);
        });
     /*var posting = $.post(url, { Email: email_, Password: password_,FirstName:firstname_, SecondName: secondname_,  Confirmpassword: confirmpassword_ });

        posting.done(function (data) {
            
                alert("Registration successful");
            });
            posting.fail(function (data, status, xhr) {
                if (xhr == "") {
                    alert("Error! Server is now unavailable.");
                } else {
                    alert("Error! Wrong user's data.");
                }
            });*/
        });


