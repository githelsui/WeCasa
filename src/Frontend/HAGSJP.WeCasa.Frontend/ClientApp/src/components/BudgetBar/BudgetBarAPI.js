import axios from 'axios';

const fetchBudgetBar = (username)  =>
{
    axios.get(`finances/${username}`).then((response) => {
        var res = response.data
        this.setState({
                        group: res["group"],
                        budget: res["budget"],
                        groupTotal: res["groupTotal"],
                        totalSpentPerMember: res["totalSpentPerMember"]
        });
    });
}
    
//     fetchTable(username) 
//     {
//         axios.get(`finances/${username}`).then((response) => {
//             var res = response.data
//             this.setState({
//                             total: res["total"],
//                             activeBills: res["activeBills"],
//                             deletedBills: res["deletedBills"]
//             });
//         });
//     }


//     persistEditForm(bill) 
//     {
//         axios.put(`finance/EditBill`).then(res => {
//             var isSuccessful = res.data;
//             if (isSuccessful) {

//             } else {
//             }
//         })
//         .catch((error) => { console.error(error) });
//     }
// }

// const submitLoginForm = (values) => {
//     setInputFailures(false);
//     var failureMessage = '';

//     let billForm = {
//         Name: values.name,
//         Description: values.description,
//         Amount: values.amount,
//         RepeatMonthly: values.repeatMonthly,
//         IsPaid: values.isPaid,
//         Payees: values.Payees,
//         fileName: values.fileName
//     };

//     // -- Client-side Input Validation

//     // Blank User Inputs
//     for (let key in billForm) {
//         if (userAccount[key] == null) {
//             setInputFailures(true);
//             failureMessage = 'Empty fields are not accepted.'
//             break;
//         }
//     }

//     if (!inputFailures) {
//         attemptInitialLogin(userAccount)
//     } else {
//         failureLoginView(failureMessage);
//     }
// };
