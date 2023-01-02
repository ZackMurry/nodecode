import { extendTheme } from '@chakra-ui/react'

const theme = extendTheme({
  colors: {
    grayBg: '#f5f5f5',
    grayBorder: '#d7d7d7',
    brightBlue: '#3d64c7',
    tealHero: '#41ab98',
    purpleHero: '#3d64c7',
    lightBlueHero: '#38739d',
    borderBlue: '#6e97b4'
  },
  fonts: {
    heading: "'Noto Sans', Sans-Serif",
    body: "'Noto Sans', Sans-Serif"
  },
  styles: {
    global: {
      'html, body': {
        backgroundColor: '#f0f0f0'
      },
      ':host,:root': {
        '--chakra-ui-focus-ring-color': '#41ab98'
      }
    }
  },
  shadows: {
    // This is also possible. Not sure I like inject this into
    // an existing theme section.
    // It creates a CSS variable named --chakra-shadows-focus-ring-color
    // 'focus-ring-color': 'rgba(255, 0, 125, 0.6)',
    outline: '0 0 0 3px var(--chakra-ui-focus-ring-color)'
  },
  config: {
    initialColorMode: 'light',
    useSystemColorMode: false
  }
})

export default theme
