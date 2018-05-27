Vue.component('my-input',{
    props:
        {
            id:String,
            pattern:{
                type:String,
                default:""
            },
            type:{
                type:String,
                default:"text"
            }
        },
     template:'<div class="control-group">\
                <label class="control-label" for="type">{{id}}</label>\
                <div class="controls">\
                    <input :type={type} :id="id" :name="id" class="input-xlarge" required :pattern="pattern" v-model="app.inputs[id]">\
                </div>\
            </div>'
    }); 

 new Vue({
    el: '#my-form',
    data: {
        outputs:{Name:"",Price:0},
        inputs:[
            {Id:"Name",name:"Product Name",pattern:"^[A-Za-z_]{1,100}$",type:"text"},
            {Id:"Price",name:"Product Price",pattern:"\[0-9]{1,10}+(\,[0-9]{1,2})?",type:"text"}
        ]
    }
    });
$("form").submit(function (event) {
        event.preventDefault();

    url = "http://localhost:50924/Product/Create";
    email_ = localStorage.Email;
    token_ = localStorage.Token;
    name_ = $('#Name').val();
    price_ = $('#Price').val();
    fetch(url,{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({Email: email_, 
            SecurityStamp: token_,
            Name: name_,
            Price: price_})
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