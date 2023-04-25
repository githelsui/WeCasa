/* global before */
/* global after */

describe('Chore List Success', function () {
    before(browser => browser.navigateTo('https://localhost:44411/'));

    it('should create an account', function (browser) {
        browser
            .waitForElementVisible('body') // wait for the body to be visible
            .setValue('#firstName', 'Jack1')
            .setValue('#lastName', 'Frost1')
            .setValue('#email', 'frost1@gmail.com')
            .setValue('#password1', 'P@ssword1234!')
            .click('button[type="submit"]')
            .waitForElementVisible('body')
            .click('div#LoginButton button.ant-btn')
            .assert.urlContains("/login")
    })

    it('should log in', function (browser) {
        browser
            .waitForElementVisible('body') // wait for the body to be visible
            .setValue('#email', 'joy@gmail.com')
            .setValue('#password', 'P@ssword1234!')
            .click('button[type="submit"]')
    })

    it('should enter the OTP code', function (browser) {
        browser.waitForElementVisible('#OTPSection h6:nth-child(2)', function () { // wait for the h6 section to be visible
            browser.getText('#OTPSection h6:nth-child(2)', function (result) { // get text from 2nd h6
                const otpCode = result.value.split(":")[1].trim() // get otp code
                browser
                    .setValue('#otp', otpCode)
                    .click('#loginForm button[type="submit"]')
                    .assert.urlContains("/home")
            });
        });
    });

    it('should enter group', function (browser) {
        browser
            .waitForElementVisible('button.ant-btn-primary') // wait for the button to be visible
            .click('button.ant-btn-primary') // click the button
            .assert.urlContains("/group-settings")
    });

    it('should navigate to chorelist tab', function (browser) {
        browser
            .waitForElementVisible('a[href="/chorelist"]') // wait for the tab to be visible
            .click('a[href="/chorelist"]') // click the tab
            .assert.urlContains("/chorelist")
    }); 

    it('Add valid chore', function (browser) {
        browser
            .waitForElementVisible('body')
            .click('button#add-chore')
            .waitForElementVisible('#chore-creation-model')
            .setValue('input[type="name"', "valid name test")
            .setValue('input[type="choreDaysReq"', ["MON", "WED"])
            .click('input[type="submit"]')
            .pause(1000)
            .waitForElementVisible('.ant-notification-notice-message')
            .getText('.ant-notification-notice-message', function (result) {
                this.assert.containsText(result.value, 'Successfully created chore')
            });
    });
    after(browser => browser.end());
});