new Vue({
    el: '#app',
    data: {
        leftName:{ContactUs:"Contact  Us"},
        leftHref:{ContactUs:"ContactUs.html"},
        rightName:{},
        rightHref:{}
    },
    created: function () {
        if(localStorage.Role == null || localStorage.Email == null || localStorage.Token == null){
            this.leftName.ContactUs="Contact Us";
            this.rightName.SignIn="Sign In";
            this.rightHref.SignIn="SignIn.html";
            this.rightName.SignUp="Sign Up";
            this.rightHref.SignUp="SignUp.html";
        } 
        else{
            url = "http://localhost:50924/Account/GetUser?Email="+localStorage.Email+"&SecurityStamp="+localStorage.Token;
            var app = this;
            fetch(url,{
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
                
            })
                .then((response) => {
                    if(response.ok) {
                        return response.json();
                    }

                    throw new Error('Network response was not ok');
                })
                .then((json) => {
                    if(json.Role==null){
                        alert(json);
                        location.href="Main.html";
                    }
                    else{

                        app.leftName.ContactUs="Contact Us";
                        if(json.Role=="user"){
                            app.leftName.MySensors="My Sensors";
                            app.leftHref.MySensors="MySensors.html";
                            app.leftName.MySurveillance="My Surveillance";
                            app.leftHref.MySurveillance="MySurveillance.html";
                            app.rightName.Setting="Setting";
                            app.rightHref.Setting="Setting.html";
                        }
                        else{
                            if(json.Role == "courier"){
                                app.leftName.MyTasks="My Tasks";
                                this.leftHref.MyTasks="MyTasks.html";
                            }
                            if(json.Role == "admin"){
                                app.leftName.AddProduct="Add Product";
                                app.leftHref.AddProduct="AddProduct.html";
                                app.leftName.AddSensor="Add Sensor";
                                app.leftHref.AddSensor="AddSensor.html";
                                app.leftName.AddCourier="Add Courier";
                                app.leftHref.AddCourier="AddCourier.html";
                                 app.leftName.Orders="Orders";
                                app.leftHref.Orders="Orders.html";
                            }
                        }
                        app.rightName.LogOut="Log Out";
                        app.rightHref.LogOut="SignIn.html";
                    }
                })
                .catch((error) => {
                    console.log(error);
                    //localStorage.clear();
                    //document.location.href="SignIn.html";
                });
                }
        
    }
    });