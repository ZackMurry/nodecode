import { Box } from '@chakra-ui/react'
import PackagePreview from 'components/PackagePreview'
import { GetServerSideProps, NextPage } from 'next'
import { PackagePreviewResponse } from 'types/package'

interface Props {
  results: PackagePreviewResponse[]
}

const SearchPage: NextPage<Props> = ({ results }) => (
  <Box p={{ base: '5vh 10vw', xl: '5vh 10vw' }}>
    {results != null &&
      results.map(p => (
        <PackagePreview packageName={p.name} version={p.latestVersion} description={p.description} key={p.name} />
      ))}
  </Box>
)

export default SearchPage

export const getServerSideProps: GetServerSideProps = async ({ query, res }) => {
  const domain = process.env.NODE_ENV === 'production' ? 'https://ncpm.zackmurry.com' : 'http://localhost'
  let search = query['q']
  console.log(search)
  if (!search) {
    res.statusCode = 302
    res.setHeader('location', '/')
    res.end()
    return
  }
  if (search instanceof Array) {
    search = search[0]
  }
  const response = await fetch(`${domain}/api/v1/packages/search?q=${encodeURIComponent(search)}`)
  const json = await response.json()
  console.log(json)
  return {
    props: {
      results: json
    }
  }
}
