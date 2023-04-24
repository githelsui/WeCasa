/* global before */
/* global after */

describe('Pre-reqs', function() {
    before(browser => browser.navigateTo('https://localhost:44411/'))

    it('should create account', function (browser) {
        browser
            .waitForElementVisible('body') 
            .setValue('#firstName', 'Frontend')
            .setValue('#lastName', 'Test')
            .setValue('#email', 'e2e@gmail.com')
            .setValue('#password1', 'P@ssword1234!')
            .click('button[type="submit"]')
            .waitForElementVisible('body')
            .click('div#LoginButton button.ant-btn')
            .assert.urlContains("/login")
    })

    it('should enter login info', function (browser) {
        browser
            .waitForElementVisible('body') 
            .setValue('#email', 'e2e@gmail.com')
            .setValue('#password', 'P@ssword1234!')
            .click('button[type="submit"]')
    })

    it('should enter the OTP code', function (browser) {
        browser.waitForElementVisible('#OTPSection h6:nth-child(2)', function () { 
            browser.getText('#OTPSection h6:nth-child(2)', function (result) { 
                const otpCode = result.value.split(":")[1].trim()
                browser
                    .setValue('#otp', otpCode)
                    .click('#loginForm button[type="submit"]')
                    .assert.urlContains("/home")
            });
        });
    });

    it('should navigate to group settings', function (browser) {
        browser
            .waitForElementVisible('button.ant-btn-primary')
            .click('button.ant-btn-primary')
            .assert.urlContains("/group-settings")
    })

    it('should render progress in members tab', function (browser) {
        browser
            .waitForElementVisible('#group-members')
            .click('#groupMembers') // go to Group Members tab
            .waitForElementVisible('#e2e@gmail.com-progressBar', 3000) // waiting 3 seconds for progress bar to render
            .assert.visible('#e2e@gmail.com-progressBar')
    })

    it('should hide progress in members tab', function (browser) {

    })

    it('should show gray bar for no chores', function (browser) {

    })

    it('should add chore', function (browser) {

    })

    it('should mark chore as complete', function (browser) {

    })

    it('should update progress bar', function (browser) {

    })
  
    after(browser => browser.end());
  });

  
  