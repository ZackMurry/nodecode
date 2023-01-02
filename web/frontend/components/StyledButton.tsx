import { Button, ButtonProps, ComponentWithAs } from '@chakra-ui/react'

const StyledButton: ComponentWithAs<'button', ButtonProps> = ({ children, ...props }) => (
  <Button
    colorScheme='teal'
    color='white'
    bg='tealHero'
    _hover={{ bgColor: 'teal.600' }}
    _active={{ bgColor: 'teal.600' }}
    {...props}
  >
    {children}
  </Button>
)

export default StyledButton
