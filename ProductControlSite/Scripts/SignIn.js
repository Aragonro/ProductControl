$(document).ready(function () {
    localStorage.clear();
    $("form").submit(function (event) {
       
        event.preventDefault();
        url = 'http://localhost/Account/Login';
        localStorage.clear();
        email_ = $('#email').val();
        password_ = $('#password').val();
        fetch(url,{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({Email: email_, 
            Password: password_})
    })
        .then((response) => {
            if(response.ok) {
                return response.json();
            }

            throw new Error('Network response was not ok');
        })
        .then((json) => {
            if(json.SecurityStamp==null){
                alert(json);
            }
            else{
                localStorage.Token=json.SecurityStamp;
                localStorage.Email=email_;
                localStorage.Role=json.Role;
                document.location.href = 'Main.html';
            }
        })
        .catch((error) => {
            console.log(error);
        });
       /* var posting = $.post(url, { grant_type: 'password', username: email_, password: password_ });

        posting.done(function (data) {
            localStorage.email = email_;
            localStorage.token = data.access_token;
            var posting1 = $.get(url1)
            posting1.done(function (data) {
                localStorage.login = data.Login;
                localStorage.id = data.UserId;
                document.location.href = 'Main.html';
                
            });
            posting1.fail(function (data, status, xhr) {
                if (xhr == "") {
                    alert("Error! Server is now unavailable.");
                } else {
                    alert("Error! Wrong user's data.");
                }
                localStorage.clear();
            });
        });

        posting.fail(function (data, status, xhr) {
            if (xhr == "") {
                alert("Error! Server is now unavailable.");
            } else {
                alert("Error! Wrong user's data.");
            }
        });*/
    });
})
