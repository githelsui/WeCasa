/* global before */
/* global after */

describe('File Upload Failure', function () {
    before(browser => browser.navigateTo('https://localhost:44411/files'));

    it('File type is invalid', function (browser) {
        browser
            .waitForElementVisible('body')
            .click('button#add-file')
            .setValue('input[type="file"', require('path').resolve(__dirname + '/files/main.css'))
            .click('input[type="submit"]')
            .pause(1000)
            .waitForElementVisible('.ant-notification-notice-message')
            .getText('.ant-notification-notice-message', function (result) {
                assert.equals(result.value, 'Invalid file type')
            });
    });
    after(browser => browser.end());
});