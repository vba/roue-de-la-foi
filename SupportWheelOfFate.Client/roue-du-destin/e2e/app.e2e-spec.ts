import { RoueDuDestinPage } from './app.po';

describe('roue-du-destin App', function() {
  let page: RoueDuDestinPage;

  beforeEach(() => {
    page = new RoueDuDestinPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
