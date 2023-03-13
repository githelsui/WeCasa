/* global before */
/* global after */

describe('Failed Login', function() {
    before(browser => browser.navigateTo('https://localhost:44411/finances'));
    
    it('Initial page state', function (browser) {
      browser.click('button#update-budget-btn');
      browser.waitForElementVisible('Modal#budget-form', 5000);
      browser.setValue('InputNumber#budget-input', '5000');

    })
    
    after(browser => browser.end());
  });
  