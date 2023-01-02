import { ExternalLinkIcon } from '@chakra-ui/icons'
import { Box, Container, Link, SimpleGrid, Stack, Text, Img } from '@chakra-ui/react'

export default function Footer() {
  return (
    <Box bg='#f5f5f5' color='gray.700'>
      <Container as={Stack} maxW={'6xl'} py={10}>
        <SimpleGrid templateColumns={{ sm: '1fr', md: '1fr 1fr' }} spacing={8}>
          <Stack>
            <Box>
              <Text fontFamily='Teko' fontSize='48px'>
                ncpm
              </Text>
            </Box>
            <Text fontSize='sm'>
              Nodecode Package Manager. Created by Daniel Huinda and Zack Murry from Central High School in Springfield,
              Missouri for the Software Development event in the 2022 National Technology Student Association Conference.{' '}
              <Link href='https://github.com/DannyTheSloth/CodeDesigner' isExternal>
                Source code
                <ExternalLinkIcon mx='2px' pb='3px' />
              </Link>
              .
            </Text>
          </Stack>
          <Link href='https://tsaweb.org/events-conferences/2022-national-tsa-conference' isExternal>
            <Img src='/tsa_dallas_banner.png' minW='250px' maxH='110px' />
          </Link>
        </SimpleGrid>
      </Container>
    </Box>
  )
}
