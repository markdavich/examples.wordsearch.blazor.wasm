import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Alphabet Soup Developer Guide',
  description: 'Developer documentation for the Alphabet Soup word search solver',
  base: '/dev-guide/',
  outDir: '../WordSearch.Web/wwwroot/dev-guide',
  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Architecture', link: '/architecture' },
      { text: 'Getting Started', link: '/getting-started' },
      { text: 'Back to App', link: '/' }
    ],
    sidebar: [
      {
        text: 'Overview',
        items: [
          { text: 'Introduction', link: '/' },
          { text: 'Getting Started', link: '/getting-started' }
        ]
      },
      {
        text: 'Architecture',
        items: [
          { text: 'Overview', link: '/architecture' },
          { text: 'Domain Layer', link: '/architecture/domain' },
          { text: 'Application Layer', link: '/architecture/application' },
          { text: 'Infrastructure Layer', link: '/architecture/infrastructure' },
          { text: 'Web Layer', link: '/architecture/web' }
        ]
      },
      {
        text: 'API Reference',
        items: [
          { text: 'Domain Types', link: '/api/domain' },
          { text: 'Services', link: '/api/services' },
          { text: 'Blazor Components', link: '/api/components' }
        ]
      },
      {
        text: 'AI Integration',
        items: [
          { text: 'Overview', link: '/ai/overview' },
          { text: 'Platforms', link: '/ai/platforms' },
          { text: 'AI Services', link: '/ai/services' }
        ]
      },
      {
        text: 'Documentation Site',
        items: [
          { text: 'VitePress Setup', link: '/vitepress' }
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/yourusername/WordSearchSolver' }
    ]
  }
})
