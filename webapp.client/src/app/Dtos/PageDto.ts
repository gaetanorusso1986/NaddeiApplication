export interface Page {
  id: string;
  title: string;
  userId: string;
  parentId?: string;
  createdAt: string;
  updatedAt: string;
  pageSections: PageSection[];
}

export interface PageSection {
  id: string;
  pageId: string;
  order: number;
  createdAt: string;
  pageContents: PageContent[];
}

export interface PageContent {
  id: string;
  sectionId: string;
  contentType: string;
  contentData: string;
  createdAt: string;
}

export interface PageDto {
  title: string;
  userId: string;
  parentId?: string | null; // Modifica per consentire 'null' oltre a 'string'
  pageSections: PageSectionDto[];
}


export interface PageSectionDto {
  order: number;
  pageContents: PageContentDto[];
}

export interface PageContentDto {
  contentType: string;
  contentData: string;
}
