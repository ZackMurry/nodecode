import { AddIcon, DownloadIcon, ExternalLinkIcon } from '@chakra-ui/icons'
import {
  Box,
  CircularProgress,
  Divider,
  Flex,
  Grid,
  GridItem,
  Heading,
  Icon,
  IconButton,
  Img,
  Link as ChakraLink,
  Text,
  useDisclosure,
  useToast
} from '@chakra-ui/react'
import VersionPreview from 'components/VersionPreview'
import { GetServerSideProps, NextPage } from 'next'
import { useEffect, useState } from 'react'
import { GoBook, GoRepo } from 'react-icons/go'
import { ImWrench } from 'react-icons/im'
import TimeAgo from 'javascript-time-ago'
import en from 'javascript-time-ago/locale/en'
import AddPackageVersionModal from 'components/AddPackageVersionModal'
import { PackageResponse } from 'types/package'
import PackageSettingsModal from 'components/PackageSettingsModal'

const timeAgo = new TimeAgo('en-US')

interface Props {
  packageName: string
}

const monthNames = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December'
]

const PackagePage: NextPage<Props> = ({ packageName }) => {
  const [pkg, setPkg] = useState<PackageResponse | null>(null)
  const { isOpen: isVersionModalOpen, onOpen: onOpenVersionModal, onClose: onCloseVersionModal } = useDisclosure()
  const { isOpen: isSettingsModalOpen, onOpen: onOpenSettingsModal, onClose: onCloseSettingsModal } = useDisclosure()

  const toast = useToast()

  const fetchPackageDetails = async () => {
    const response = await fetch(`/api/v1/packages/name/${packageName}`)
    if (response.ok) {
      setPkg(await response.json())
    } else {
      toast({
        title: 'Error',
        description: `Error fetching package data: ${response.status}`,
        status: 'error',
        duration: 4000,
        isClosable: true
      })
    }
  }

  useEffect(() => {
    fetchPackageDetails()
  }, [])

  if (pkg === null) {
    return (
      <Flex justifyContent='center' alignItems='center'>
        <CircularProgress />
      </Flex>
    )
  }

  const accountCreatedTime = new Date(pkg.author.timeCreated)
  const deltaT = timeAgo.format(new Date(pkg.lastUpdated), 'twitter')

  return (
    <Box p='3.5vh 10vw'>
      <Flex justifyContent='space-between' alignItems='center'>
        <Heading as='h1'>
          {packageName} {pkg.latestVersion !== null ? pkg.latestVersion : '(no versions available)'}
        </Heading>
        {pkg.isOwnedByUser && (
          <IconButton size='lg' aria-label='Edit' icon={<ImWrench fontSize='24px' />} onClick={onOpenSettingsModal} />
        )}
      </Flex>
      <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' my='25px' />
      <Grid templateColumns={{ base: 'repeat(1, 1fr)', lg: 'repeat(3, 1fr)' }}>
        <GridItem colSpan={{ base: 1, md: 2 }}>
          <Text fontSize='16px'>{pkg.description}</Text>
          <Text fontStyle='italic' color='gray.500' mt='5px'>
            Last updated {deltaT} ago
          </Text>
          <Flex mt='15px' alignItems='center'>
            <DownloadIcon fontSize='18px' />
            <Text fontSize='16px' ml='15px'>
              {pkg.downloads} download
              {pkg.downloads !== 1 ? 's' : ''}
            </Text>
          </Flex>
          {pkg.repositoryUrl && (
            <Flex mt='10px' alignItems='center'>
              <Icon aria-label='Repository URL' as={GoRepo} fontSize='18px' />
              <ChakraLink fontSize='16px' ml='15px' href={pkg.repositoryUrl} isExternal>
                Repository
              </ChakraLink>
              <ExternalLinkIcon fontSize='14px' pb='2px' mx='4px' />
            </Flex>
          )}
          {pkg.documentationUrl && (
            <Flex mt='10px' alignItems='center'>
              <Icon as={GoBook} fontSize='18px' />
              <ChakraLink fontSize='16px' ml='15px' href={pkg.documentationUrl} isExternal>
                Documentation
              </ChakraLink>
              <ExternalLinkIcon fontSize='14px' pb='2px' mx='4px' />
            </Flex>
          )}
          <Box mt='35px'>
            <Flex justifyContent='space-between' alignItems='center'>
              <Heading fontSize='22px'>Versions</Heading>
              {pkg.isOwnedByUser && (
                <IconButton aria-label='Add' size='lg' icon={<AddIcon />} onClick={onOpenVersionModal} />
              )}
            </Flex>

            <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' mt='10px' mb='20px' />
            <Box>
              {pkg.versions.map(v => (
                <VersionPreview
                  packageName={packageName}
                  version={v.version}
                  uploadedTime={v.timePublished}
                  isSmall
                  key={v.version}
                />
              ))}
            </Box>
          </Box>
        </GridItem>
        <GridItem mt={{ base: '35px', lg: '0px' }} pl={{ base: '0px', lg: '10px' }}>
          <Grid
            templateColumns={{
              base: 'repeat(1, 1fr)',
              md: 'repeat(3, 1fr)',
              lg: 'repeat(1, 1fr)',
              xl: 'repeat(3, 1fr)'
            }}
          >
            <GridItem p='3px'>
              <Img src={pkg.author.avatarUrl} borderRadius='25%' w='100%' maxW='125px' />
            </GridItem>
            <GridItem colSpan={{ base: 1, md: 2, lg: 1, xl: 2 }} p='8px 3px'>
              <Flex flexDir='column' justifyContent='space-between' h='100%'>
                <Box>
                  <Heading fontSize='22px' fontWeight='normal'>
                    {pkg.author.username}
                  </Heading>
                  <Text fontWeight='light' color='gray.400'>
                    AUTHOR
                  </Text>
                  <Text mt='5px' color='gray.700'>
                    {pkg.author.numPackages} package{pkg.author.numPackages !== 1 ? 's' : ''} created
                  </Text>
                </Box>
                <Box pb='5px'>
                  <Text fontStyle='italic' color='gray.500'>
                    Creator since {monthNames[accountCreatedTime.getMonth()]} {accountCreatedTime.getFullYear()}
                  </Text>
                </Box>
              </Flex>
            </GridItem>
          </Grid>
        </GridItem>
      </Grid>
      <AddPackageVersionModal isOpen={isVersionModalOpen} onClose={onCloseVersionModal} packageName={packageName} />
      <PackageSettingsModal isOpen={isSettingsModalOpen} onClose={onCloseSettingsModal} pkg={pkg} />
    </Box>
  )
}

export default PackagePage

export const getServerSideProps: GetServerSideProps<Props> = async ({ query }) => ({
  props: {
    packageName: typeof query.package === 'string' ? query.package : query.package[0]
  }
})
