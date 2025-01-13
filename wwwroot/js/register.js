const registerForm = createApp({
    data() {
        return {
            registerFormTitle: "會員註冊",
            registerFormDetails: [
                {
                    field: "name",
                    nameGroups: {
                        lastName: {
                            label: "姓", labelFor: "lastName", formValue: ""
                        },
                        firstName: {
                            label: "名", labelFor: "firstName", formValue: ""
                        }
                    }
                },
                { field: "email", label: "E-mail", labelFor: "email", formValue: "", type: "email", autocomplete: "email", placeholder: "xxxxxxxx@example.com" },
                { field: "password", label: "密碼", labelFor: "password", type: "password", autocomplete: "new-password", formValue: "" },
                { field: "confirmPassword", label: "再次確認密碼", labelFor: "confirmPassword", type: "password", autocomplete: "new-password", formValue: "" },
                { field: "phoneNumber", label: "手機號碼", labelFor: "phoneNumber", formValue: "", type: "tel", autocomplete: "tel", placeholder: "09XXXXXXXX" },
                {
                    field: "yearMonthDay",
                    birthDateGroups: {
                        year: {
                            label: "年", labelFor: "year", yearList: [], formValue: 0
                        },
                        month: {
                            label: "月", labelFor: "month", monthList: [], formValue: 0
                        },
                        day: {
                            label: "日", labelFor: "day", dayList: [], formValue: 0
                        }
                    }
                }
            ],
            backBtn: "回上一頁",
            sendBtn: "送出",
            errors: {}
        }
    },
    methods: {
        async submitForm() {
            const apiURL = "/api/UserApi/Register";
            const registerForm = this.generateRegisterForm();   

            try {
                const response = await axios.post(apiURL, registerForm);
                if (response && response.data.success === 'true') {
                }
            }
            catch (error) {                                
                if (error.response.data.success === false && error.response.status === 400) {
                    const formErrors = error.response.data.errors;
                    console.log(formErrors);
                    sessionStorage.setItem('formErrors', JSON.stringify(formErrors));
                    //window.location.reload();
                }
            }
        },
        async getRegisterForm() {
            const apiURL = "/api/UserApi/GetRegisterForm";

            await axios.get(apiURL)
                .then(response => {
                    if (response) {
                        const registerForm = response.data.registerForm;
                        if (registerForm) {
                            //console.log('register form :', response.data.success);
                            //console.log(registerForm);
                            //this.formData = registerForm;
                        }
                    }
                })
                .catch(error => {
                })
        },
        async saveRegisterForm() {
            const apiURL = "/api/UserApi/SaveRegisterForm";

            await axios.post(apiURL, this.formData)
                .then(response => {
                    if (response) {
                    }
                })
                .catch(error => {
                });
        },
        initialYearList() {
            // Find the object containing the year, month, day options.
            const birthDateGroups = this.registerFormDetails.find(field => field.field === "yearMonthDay").birthDateGroups;

            // Generate a list of years from the beginning of a specified year to the end of a specified year.
            // For example, 1930 - 2024.
            const yearList = Array.from({ length: 95 }, (_, i) => {
                let year = 1930 + i;
                return {
                    yearValue: year,
                    yearOption: year
                }
            });

            // Set the default option "請選擇年份";
            const defaultParams = {
                yearValue: 0,
                yearOption: "請選擇年份"
            };

            // Insert the default option at the beginning of the year list.
            yearList.unshift(defaultParams);
            // Assin the generated year list to birthDateGroups.year.yearList, which will be used for the year dropdwon.
            birthDateGroups.year.yearList = yearList;
            //console.log(yearList);
        },
        initialMonthList() {

            const birthDateGroups = this.registerFormDetails.find(field => field.field === "yearMonthDay").birthDateGroups;
            const year = birthDateGroups.year.formValue;

            const defaultParams = {
                monthValue: 0,
                monthOption: "請選擇月份"
            };

            if (year <= 0) {
                birthDateGroups.month.formValue = 0;
                birthDateGroups.month.monthList = [defaultParams];
                this.initialDayList();
                //console.log(birthDateGroups);
            }
            else {
                const monthList = Array.from({ length: 12 }, (_, i) => {
                    let month = i + 1;
                    return {
                        monthValue: month,
                        monthOption: month
                    }
                });
                monthList.unshift(defaultParams);
                birthDateGroups.month.monthList = monthList;
            }
            //console.log(birthDateGroups.month.monthList);
        },
        initialDayList() {
            const birthDateGroups = this.registerFormDetails.find(field => field.field === "yearMonthDay").birthDateGroups;
            const year = birthDateGroups.year.formValue;
            const month = birthDateGroups.month.formValue;

            const defaultParams = {
                dayValue: 0,
                dayOption: "請選擇日期"
            };

            if (year <= 0 || month <= 0) {
                birthDateGroups.day.formValue = 0;
                birthDateGroups.day.dayList = [defaultParams];
            }
            else {
                const days = new Date(year, month, 0).getDate();
                const dayList = Array.from({ length: days }, (_, i) => {
                    let day = i + 1;
                    return {
                        dayValue: day,
                        dayOption: day
                    }
                });
                dayList.unshift(defaultParams);
                birthDateGroups.day.dayList = dayList;
            }
            //console.log(birthDateGroups.day.dayList);
        },
        generateRegisterForm() {
            const registerForm = {};

            this.registerFormDetails.forEach(item => {
                if (item.field === "name") {
                    registerForm.lastName = item.nameGroups.lastName.formValue;
                    registerForm.firstName = item.nameGroups.firstName.formValue;
                }
                else if (item.field === "yearMonthDay") {
                    registerForm.year = item.birthDateGroups.year.formValue;
                    registerForm.month = item.birthDateGroups.month.formValue;
                    registerForm.day = item.birthDateGroups.day.formValue;
                }
                else {
                    registerForm[item.field] = item.formValue;
                }
            });
            //console.log(registerForm);
            return (registerForm);
        },
    },
    async created() {
        //await this.getRegisterForm();
        //console.log(this.formData);
    },
    mounted() {
        this.initialYearList();
        this.initialMonthList();
        this.initialDayList();
        const errors = sessionStorage.getItem('formErrors');
        //if (errors) {
        //    this.errors = errors
        //    console.log(this.errors);
        //}
        //this.generateRegisterForm();
        //console.log(this.registerFormDetails[5].birthDateGroups);
    }
});
registerForm.mount('#register');