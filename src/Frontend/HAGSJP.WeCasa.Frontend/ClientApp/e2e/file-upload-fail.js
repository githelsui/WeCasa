/* global before */
/* global after */

describe('File Upload Success', function () {
    before(browser => browser.navigateTo('https://localhost:44411/files'));

    it('Add test file', function (browser) {
        browser
            .waitForElementVisible('body')
            .click('button#add-file')
            .setValue('input[type="file"', require('path').resolve(__dirname + '/files/test.txt'))
            .click('input[type="submit"]')
            .pause(1000)
            .waitForElementVisible('.ant-notification-notice-message')
            .getText('.ant-notification-notice-message', function (result) {
                this.assert.containsText(result.value, 'successfully uploaded')
            });
    });
    after(browser => browser.end());
});