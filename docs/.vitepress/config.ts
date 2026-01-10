import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Alphabet Soup User Guide',
  description: 'User guide for the Alphabet Soup word search solver',
  base: '/guide/',
  outDir: '../WordSearch.Web/wwwroot/guide',
  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Getting Started', link: '/getting-started' },
      { text: 'Back to App', link: '/' }
    ],
    sidebar: [
      {
        text: 'Guide',
        items: [
          { text: 'Introduction', link: '/' },
          { text: 'Getting Started', link: '/getting-started' },
          { text: 'File Format', link: '/file-format' },
          { text: 'Features', link: '/features' },
          { text: 'AI Image Parsing', link: '/ai-parsing' }
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/yourusername/WordSearchSolver' }
    ]
  }
})
