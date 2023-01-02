import {
  Box,
  Button,
  CircularProgress,
  Divider,
  Flex,
  Heading,
  Img,
  useBreakpointValue,
  useToast,
  Grid,
  GridItem,
  IconButton,
  Tooltip,
  useDisclosure
} from '@chakra-ui/react'
import { NextPage } from 'next'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import PackagePreview from 'components/PackagePreview'
import { AddIcon } from '@chakra-ui/icons'
import { UserPrincipalResponse } from 'types/user'
import { PackagePreviewResponse } from 'types/package'
import ConfirmationModal from 'components/ConfirmationModal'

const AccountPage: NextPage = () => {
  const [account, setAccount] = useState<UserPrincipalResponse | null>(null)
  const [packages, setPackages] = useState<PackagePreviewResponse[] | null>(null)

  const { isOpen: isDeleteModalOpen, onOpen: onOpenDeleteModal, onClose: onCloseDeleteModal } = useDisclosure()
  const router = useRouter()
  const toast = useToast()
  const isDesktopView = useBreakpointValue({ base: false, lg: true })

  const getAccount = async () => {
    const accountPromise = fetch('/api/v1/users/user')
    const packagesPromise = fetch('/api/v1/users/user/packages')
    let accountResponse: Response | null, packagesResponse: Response | null
    try {
      ;[accountResponse, packagesResponse] = await Promise.all([accountPromise, packagesPromise])
    } catch {
      router.push('/api/v1/oauth2/code/github')
    }
    if (!accountResponse) {
      router.push('/api/v1/oauth2/code/github')
      return
    }
    if (!accountResponse.ok) {
      if (accountResponse.status === 403) {
        router.push('/api/v1/oauth2/code/github')
      } else {
        toast({
          title: 'Error',
          description: `Error fetching account data: ${accountResponse.status}`,
          status: 'error',
          duration: 9000,
          isClosable: true
        })
        return
      }
    }
    if (!packagesResponse || !packagesResponse.ok) {
      toast({
        title: 'Error',
        description: `Error fetching packages: ${packagesResponse.status}`,
        status: 'error',
        duration: 4000,
        isClosable: true
      })
      const accountJson = await accountResponse.json()
      setAccount(accountJson)
      return
    }

    const [accountJson, packagesJson] = await Promise.all([accountResponse.json(), packagesResponse.json()])
    setPackages(packagesJson)
    setAccount(accountJson)
  }

  useEffect(() => {
    getAccount()
  }, [])

  if (account === null) {
    return (
      <Flex justifyContent='center' alignItems='center'>
        <CircularProgress />
      </Flex>
    )
  }

  const handleDeleteAccount = async () => {
    console.log('delete')
    const response = await fetch('/api/v1/users/user', { method: 'DELETE' })
    if (response.ok) {
      router.push('/')
    } else {
      toast({
        title: 'Error',
        description: `Error deleting account: ${response.status}`,
        status: 'error',
        duration: 7000,
        isClosable: true
      })
    }
  }

  const { avatarUrl, name } = account

  return (
    <Grid templateColumns={{ base: 'repeat(1, 1fr)', md: 'repeat(4, 1fr)' }} padding={{ base: '3vh 10vw', xl: '8vh 10vw' }}>
      <GridItem>
        <Box width={{ base: '80vw', md: '17.5vw', xl: '13vw' }}>
          <Img src={avatarUrl} borderRadius='25%' w={{ base: '50%', lg: '80%' }} />
          <Heading w='100%' pt='1vh'>
            {name}
          </Heading>
          {isDesktopView && <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' m='25px 0' />}
          <Button variant='outline' colorScheme='red' mt='5px' onClick={onOpenDeleteModal}>
            Delete Account
          </Button>
        </Box>
        {!isDesktopView && <Divider color='#d5d5d5' opacity='1' borderColor='rgba(0,0,0,0.1)' mt='25px' />}
      </GridItem>
      <GridItem colSpan={{ base: 1, md: 3 }} pr={isDesktopView ? '10vw' : '0'} mt={isDesktopView ? 0 : '2vh'}>
        <Flex justifyContent='space-between'>
          <Heading>Packages</Heading>
          <Tooltip label='Create package'>
            <IconButton aria-label='Create package' icon={<AddIcon />} size='lg' onClick={() => router.push('/create')} />
          </Tooltip>
        </Flex>
        <Box>
          {packages != null &&
            packages.map(p => (
              <PackagePreview packageName={p.name} version={p.latestVersion} description={p.description} key={p.name} />
            ))}
        </Box>
      </GridItem>
      <ConfirmationModal
        isOpen={isDeleteModalOpen}
        title='Delete Account'
        description='Are you sure you would like to delete your account?'
        onClose={onCloseDeleteModal}
        onConfirm={handleDeleteAccount}
      />
    </Grid>
  )
}

export default AccountPage
