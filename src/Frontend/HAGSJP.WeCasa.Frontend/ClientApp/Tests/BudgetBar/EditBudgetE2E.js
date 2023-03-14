/* global before */
/* global after */

describe('Failed Login', function() {
    before(browser => browser.navigateTo('http://localhost:44411'));
    
    it('Navigate to Login', function (browser) {
      browser.click('button#LoginButton');
      // browser.clearValue('input[name="email"]')
      browser.setValue('input[name="email"]', 'frost@gmail.com');
      browser.setValue('input[name="password"]', 'P@ssword1234');
      browser.click('button#submit');
      browser.setValue('input[name="otp"]', 'P@ssword1234');
      
      // browser.click('button#update-budget-btn');
      // browser.waitForElementVisible('Modal#budget-form', 5000);
      browser.setValue('InputNumber#budget-input', '5000');

    })
    
    after(browser => browser.end());
  });
  