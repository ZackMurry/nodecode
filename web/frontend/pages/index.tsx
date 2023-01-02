import type { GetStaticProps, NextPage } from 'next'
import { Box, Heading, SimpleGrid } from '@chakra-ui/react'
import PackagePreview from 'components/PackagePreview'
import Footer from 'components/Footer'
import { PackagePreviewResponse } from 'types/package'

interface Props {
  popularPackages: PackagePreviewResponse[]
  newPackages: PackagePreviewResponse[]
}

const Home: NextPage<Props> = ({ popularPackages, newPackages }) => {
  return (
    <Box>
      <Box className='moving-gradient' p={{ base: '5vh 10vw', xl: '15vh 10vw' }}>
        {/* random taglines idk */}
        <Heading color='white' fontSize='42pt' mt='5vh'>
          Simplify your code
        </Heading>
        <Heading color='white' fontStyle='normal' fontSize='18pt' mt='3vh'>
          Use Nodecode Package Manager to reduce the amount of code in your project.
        </Heading>
      </Box>

      <SimpleGrid columns={{ sm: 1, xl: 2 }} m='15vh 10vw'>
        <Box>
          <Heading>Popular Packages</Heading>
          <Box pr='50px'>
            {popularPackages !== null &&
              popularPackages.map(p => (
                <PackagePreview packageName={p.name} version={p.latestVersion} description={p.description} key={p.name} />
              ))}
          </Box>
        </Box>
        <Box>
          <Heading>New Packages</Heading>
          <Box>
            {newPackages !== null &&
              newPackages.map(p => (
                <PackagePreview packageName={p.name} version={p.latestVersion} description={p.description} key={p.name} />
              ))}
          </Box>
        </Box>
      </SimpleGrid>

      <Footer />
    </Box>
    // todo: add footer w/ github, statement that it's for TSA, creators, download link, etc
  )
}

export default Home

export const getStaticProps: GetStaticProps<Props> = async () => {
  console.log('getStaticProps')
  const domain = process.env.NODE_ENV === 'production' ? 'https://ncpm.zackmurry.com' : 'http://localhost'
  // const domain = 'http://localhost'
  let response: Response
  let json: { popular: PackagePreviewResponse[]; latest: PackagePreviewResponse[] }
  try {
    response = await fetch(`${domain}/api/v1/packages/trending`)
    json = await response.json()
  } catch (e) {
    return {
      props: {
        popularPackages: [],
        newPackages: []
      },
      revalidate: 1
    }
  }

  return {
    props: {
      popularPackages: json.popular,
      newPackages: json.latest
    },
    revalidate: 60
  }
}
