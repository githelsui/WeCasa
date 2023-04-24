/* global before */
/* global after */

describe('Circular Progress Bar Frontend Test Suite', function () {
    before(browser => browser.navigateTo('https://localhost:44411/'));

    // initialize
    it('should create account', function (browser) {
        browser
            .waitForElementVisible('body')
            .setValue('#firstName', 'Frontend')
            .setValue('#lastName', 'Test')
            .setValue('#email', 'e2e@gmail.com')
            .setValue('#password1', 'P@ssword1234!')
            .setValue('#password2', 'P@ssword1234!')
            .click('button[type="submit"]')
            .waitForElementVisible('body')
            .click('div#LoginButton button.ant-btn')
            .assert.urlContains("/login")
    });

    it('should enter login info', function (browser) {
        browser
            .waitForElementVisible('body')
            .setValue('#email', 'e2e@gmail.com')
            .setValue('#password', 'P@ssword1234!')
            .click('button[type="submit"]')
    });

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

    it('should create group with progress bar', function (browser) {
        browser
            .waitForElementVisible('#createGroup')
            .click('#createGroup')
            .pause(5000)
            .setValue('#groupName', 'Nightwatch group')
            .click('button[type="submit"]')
            .waitForElementVisible("#[data-key='view'") // group card with view button
            .assert.visible("#[data-key='view'")
    });

    it('should navigate to group settings', function (browser) {
        browser
            .waitForElementVisible('button.ant-btn-primary') // clicking view group
            .click('button.ant-btn-primary')
            .assert.urlContains("/group-settings")
    });

    it('should render progress in members tab', function (browser) {
        browser
            .waitForElementVisible('#group-members')
            .click('#groupMembers') // go to Group Members tab
            .waitForElementVisible('#e2e@gmail.com-progressBar', 3000) // waiting 3 seconds for progress bar to render
            .assert.visible('#e2e@gmail.com-progressBar')
    });

    /*
    it('should add chore', function (browser) {

    })

    it('should mark chore as complete', function (browser) {

    })

    it('should update progress bar', function (browser) {

    })
    */

    it('should hide progress in members tab on disable', function (browser) {
        browser
            .waitForELementVisible('#group-settings')
            .click('#group-settings') // go to GroupSettingsTab
            .waitForElementVisible('#progressBarSwitch')
            .click('#progressBarSwitch') // turning off circular progress bar feature
            .click('button[type="submit"]')
            .click('#group-members')
            .expect.element('#e2e@gmail.com-progressBar').to.not.be.visible
    });

    // cleanup
    it('should delete group', function (browser) { 
        browser
            .waitForELementVisible('#group-settings')
            .click('#group-settings') // go to GroupSettingsTab
            .waitForElementVisible('#deleteGroup') // delete group button
            .click('#deleteGroup')
            .pause(5000)
            .waitForElementVisible('button.ant-btn-primary')
            .click('button.ant-btn-primary')
            .getText('.ant-notification-notice-message', function (result) {
                assert.equals(result.value, 'Successfully deleted group.')
            })
    });

    it('should delete account', function (browser) {
        browser
            // deleting account
            .waitForElementVisible('#profile-icon')
            .click('#profile-icon')
            .waitForElementVisible('#accountSettings')
            .click('#accountSettings')
            .waitForElementVisible('body')
            .waitForElementVisible('#deleteAccount')
            .click('#deleteAccount')
            .pause(5000)
            .click('button.ant-btn-primary')
            .getText('.ant-notification-notice-message', function (result) {
                assert.equals(result.value, 'Account Deletion Successful')
            })
    });

    after(browser => browser.end());

});
  
  