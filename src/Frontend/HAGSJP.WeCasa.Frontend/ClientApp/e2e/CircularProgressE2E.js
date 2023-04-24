/* global before */
/* global after */

describe('Pre-reqs', function() {
    before(browser => browser.navigateTo('https://localhost:44411/'));

  
    after(browser => browser.end());
  });

  
  