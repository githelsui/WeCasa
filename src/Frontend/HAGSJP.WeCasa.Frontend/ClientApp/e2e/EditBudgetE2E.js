/* global before */
/* global after */

describe('Failed Login', function() {
    before(browser => browser.navigateTo('http://localhost:44411'));
    
    it('Navigate to Login', function (browser) {
      browser
      .waitForElementVisible('body', 5000)
      .setValue('input[type=email]', 'frost@gmail.com')
      .click('div[id=LoginButton]')
      .pause(2000)
      .end();
      // waitForElementVisible('button[class="ant-btn css-dev-only-do-not-override-xoi1ki ant-btn-default"]', 5000)
      // .click('.ant-btn')
      // .pause(2000)
      // .assert.urlEquals('http://localhost:44411//login')
      // .end();
      // browser.click('button#LoginButton');
      // browser.clearValue('input[name="email"]')
      // browser.setValue('input[name="email"]', 'frost@gmail.com');
      // browser.setValue('input[name="password"]', 'P@ssword1234');
      // browser.click('button#submit');
      // browser.setValue('input[name="otp"]', 'P@ssword1234');
      
      // browser.click('button#update-budget-btn');
      // browser.waitForElementVisible('Modal#budget-form', 5000);
      browser.setValue('InputNumber#budget-input', '5000');

    })
    
    after(browser => browser.end());
  });
  