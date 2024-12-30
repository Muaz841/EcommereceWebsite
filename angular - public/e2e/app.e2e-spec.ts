import { MAINABPTemplatePage } from './app.po';

describe('MAINABP App', function() {
  let page: MAINABPTemplatePage;

  beforeEach(() => {
    page = new MAINABPTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
