
// export var BudgetBarFrontendResponse = {
//     username: "",
//     group: [],
//     budget: 0,
//     groupTotal: 0,
//     totalSpentPerMember: [{
//         username: "",
//         total: 0
//     }],
//     total: 0,
//     activeBills: B
//     deletedBills: [Bill]
// }

export var Bill = {
    username: String,
    billId: String,
    dateEntered: Date,
    billName: String,
    billDescription: String,
    amount: Number,
    paymentStatus: Boolean,
    isRepeated: Boolean,
    isDeleted: Boolean,
    dateDeleted: Date,
    photoFileName: String
}


