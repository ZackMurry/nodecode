import { FC } from 'react'
import { Input, InputProps } from '@chakra-ui/react'

const color = 'borderBlue'

const StyledInput: FC<InputProps> = props => (
  <Input
    _hover={{ borderColor: color }}
    _active={{ borderColor: color }}
    _focus={{ borderColor: 'purpleHero' }}
    _selected={{ borderColor: color }}
    borderColor='gray.300'
    {...props}
  />
)

export default StyledInput
