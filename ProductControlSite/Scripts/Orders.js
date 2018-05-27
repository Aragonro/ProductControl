new Vue({
    el: '#my-form',
    data: {
        courier:"courier",
        date:"date",
        outputs:[],
        outputsName:{
            Id:"Order Id",
            DeliveryAddress:"Delivery Address",
            ProductName:"Product Name",
            CountProduct:"Count Product for Delivery",
            Price:"Order Price",
            CustomerEmail:"Customer Email"
        }
    },
    methods:{
        postOrder:function(id){
            var url="http://localhost:50924/Order/SetCourier";
            var courierEmail_ = $('#courier'+id).val();
            var deliveryDate_ = $('#date'+id).val();
            fetch(url,{
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({Id:id,
                    CourierEmail:courierEmail_,
                    DeliveryDate:deliveryDate_,
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

                    alert(json);;
                    document.location.href="Orders.html"
                })
                .catch((error) => {
                    console.log(error);
                });
        }
    },
    created(){
        var url1 = "http://localhost:50924/Order/GetAllOrdersForAdmin";
        var url2 ="?Email="+localStorage.Email+"&SecurityStamp="+localStorage.Token;
        var app = this;
        fetch(url1+url2,{
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then((response) => {
            if(response.ok) {
                return response.json();
            }

            throw new Error('Network response was not ok');
        })
        .then((json) => {
            if(typeof json == "string")
                alert(json);
            else{
                json.forEach(function(item, i, arr) {
                    
                    app.outputs.push({
                        Id:item.Id,
                        DeliveryAddress:item.DeliveryAddress,
                        CountProduct:item.CountProduct,
                        ProductName:item.ProductName,
                        CustomerEmail:item.CustomerEmail,
                        Price:item.Price
                    });
                });
            }
        })
        .catch((error) => {
            console.log(error);
        });
    }
    });