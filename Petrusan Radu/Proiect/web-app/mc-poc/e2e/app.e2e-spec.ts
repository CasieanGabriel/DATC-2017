import { McPocPage } from './app.po';

describe('mc-poc App', () => {
  let page: McPocPage;

  beforeEach(() => {
    page = new McPocPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
