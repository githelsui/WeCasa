/* global before */
/* global after */

describe('Pre-reqs', function() {
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
      browser.waitForElementVisible('#OTPSection h6:nth-child(2)', function() { // wait for the h6 section to be visible
        browser.getText('#OTPSection h6:nth-child(2)', function(result) { // get text from 2nd h6
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

    it('should navigate to finances tab', function (browser) { 
      browser
        .waitForElementVisible('a[href="/finances"]') // wait for the tab to be visible
        .click('a[href="/finances"]') // click the tab
        .assert.urlContains("/finances")
    }); 

    // it('should successfully update budget', function (browser) { 
    //   browser
    //     .waitForElementVisible('#update-budget-btn') // Wait for the button to be visible
    //     .click('#update-budget-btn') // Click the update budget button
    //     .waitForElementVisible('div.ant-modal-content')
    //     .setValue('input#input-number', '6000')
    //     // .moveToElement('.ant-modal-wrap', 4, 15)
    //     // .moveToElement("div.ant-modal-wrap", 4, 15)
    //     // .click('button.ant-modal-close') // click Save button
    //      .moveToElement('div.ant-modal-root', 4, 15)
    //     .click('button.ant-modal-close')
    //     .pause(1000)
    //     .click('.ant-modal-wrap .ant-modal-content button[type="save"]')
    //     .getText('p strong', function(result) {
    //       const budget = result.value.split("$")[1].trim() // view budget
    //       browser.assert.equal(budget, '6000')
    //     })
    // }); 

    // it('should successfully delete bill', function (browser) { 
      // browser
      // .click('//td[contains(text(), "Groceries")]/following-sibling::td//button[@data-icon="delete"]')
      // .click('div[data-node-key="history"]')
      // .assert('')
    // }); 

  
    after(browser => browser.end());
  });

  
  