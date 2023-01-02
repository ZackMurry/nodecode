import { FC, FormEvent, useState } from 'react'
import { Flex, Input, Text } from '@chakra-ui/react'
import { useRouter } from 'next/router'
import NavBarProfile from './NavBarProfile'
import Link from 'next/link'

const NavBar: FC = () => {
  const [searchValue, setSearchValue] = useState('')
  const router = useRouter()

  const onSearch = (e: FormEvent) => {
    e.preventDefault()
    router.push(`/search?q=${encodeURIComponent(searchValue)}`)
  }

  return (
    <Flex pl='10vw' pr='10vw' w='100%' alignItems='center' justifyContent='space-between' bgColor='#f5f5f5'>
      <Link href='/'>
        <a>
          <Text fontFamily='Teko' fontSize='48px'>
            ncpm
          </Text>
        </a>
      </Link>
      {/* todo: move search bar somewhere else if on mobile */}
      <form onSubmit={onSearch}>
        <Input
          ml='5vw'
          w='55vw'
          h='5vh'
          borderColor='grayBorder'
          _hover={{ borderColor: 'brightBlue' }}
          _selected={{ borderColor: 'brightBlue' }}
          _active={{ borderColor: 'brightBlue' }}
          fontSize='18px'
          placeholder='Search packages...'
          value={searchValue}
          onChange={e => setSearchValue(e.target.value)}
        />
      </form>
      <NavBarProfile />
    </Flex>
  )
}

export default NavBar
