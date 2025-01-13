const loginForm = createApp({
    data() {
        return {
            registerFormTitle: "登入",
            loginFormDetails: [
                { field: "email", labelFor: "Email", type: "email", autocomplete: "email", formValue: "" },
                { field: "password", labelFor: "密碼", type: "password", autocomplete: "new-passwrod", formValue: "" }
            ],
            loginBtn: "登入",
            forgetPasswordLink: "忘記密碼",
            registerLink: "註冊",
            facebookLogin: "Facebook",
            googleLogin: "Google",
            errors: {}
        }
    },
    methods: {
        async login() {
            const loginForm = {};

            this.loginFormDetails.forEach((item) => {
                loginForm[item.field] = item.formValue;
            });
            //console.log(loginForm);

            const apiURL = "/api/UserApi/Login";
            try {
                const response = await axios.post(apiURL, loginForm);
                //console.log(response.data);
                if (response && response.data.success == true) {
                    window.location.href = response.data.href;
                    //console.log("Login status : ", response.data.success );
                }
            }
            catch (error) {
                if (error.response.data.success === false && error.response.status === 400) {
                    const errorsForm = error.response.data.errors;
                    if (!sessionStorage.getItem(errorsForm)) {
                        sessionStorage.setItem('errorsForm', JSON.stringify(errorsForm));
                    }                                                     
                }
            }
        },
        forgetPassword() {
            const href = "/User/ForgetPassword";
            window.location.href = href;
        },
        register() {
            const href = "/User/Register";
            window.location.href = href;
        },
        getErrors() {
            const errors = sessionStorage.getItem('errorsForm');
            console.log(JSON.parse(errors));
            if (errors) {
                this.errors = JSON.parse(errors);
                sessionStorage.removeItem('errorsForm');
                //console.log(this.errors);
            }
        }
    },
    created() {
        this.getErrors();
    }
});
loginForm.mount('#login');