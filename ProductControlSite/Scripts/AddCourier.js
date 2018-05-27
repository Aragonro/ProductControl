new Vue({
    el: '#my-form',
    data: {
        inputs:[
            {Id:"Email",name:"Email",type:"email"},
            {Id:"FirstName",name:"First Name",pattern:"^[A-Za-z_]{1,100}$",type:"text"},
            {Id:"SecondName",name:"Second Name",pattern:"^[A-Za-z_]{1,100}$",type:"text"},
            {Id:"Password",name:"Password",pattern:"(?=^.{6,12}$)(?=.*\\d)(?=.*\\W+)(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$",type:"password",autocomplete:"off"},
            {Id:"ConfirmPassword",name:"Confirm Password",type:"password",autocomplete:"off"}
        ]
    }
    });
$("form").submit(function (event) {
        event.preventDefault();

    url = "http://localhost:50924/Account/RegisterCourier";
    email_ = $('#Email').val();
    firstname_=$('#FirstName').val();
    secondname_=$('#SecondName').val();
    password_ = $('#Password').val();
    confirmpassword_ = $('#ConfirmPassword').val();
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
            EmailAdmin:localStorage.Email,
            SecurityStamp:localStorage.Token})
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
});