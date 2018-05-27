new Vue({
    el: '#my-form',
    data: {
        inputs:[
            {Id:"Email",name:"Owner Email",type:"email"},
            {Id:"Name",name:"Sensor Name",pattern:"^[A-Za-z0-9]{1,100}$",type:"text"},
            {Id:"Product",name:"Product Name",pattern:"^[A-Za-z]{1,100}$",type:"text"},
            {Id:"DeliveryAddress",name:"Product Delivery Address",type:"text"},
        ]
    }
    });
$("form").submit(function (event) {
        event.preventDefault();

    url = "http://localhost:50924/Sensor/Create";
    email_ = $('#Email').val();
    name_=$('#Name').val();
    product_=$('#Product').val();
    deliveryaddress_=$('#DeliveryAddress').val();
    fetch(url,{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({Email: email_, 
            Product:product_,
            Name:name_,
            DeliveryAddress :     deliveryaddress_,    
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