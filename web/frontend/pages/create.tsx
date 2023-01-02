import { Box, Heading, Tab, TabList, TabPanel, TabPanels, Tabs } from '@chakra-ui/react'
import CreateFirstVersionForm from 'components/CreateFirstVersionForm'
import CreatePackageForm from 'components/CreatePackageForm'
import checkSession from 'lib/checkSession'
import { NextPage } from 'next'
import { useRouter } from 'next/router'
import { FC, useEffect, useState } from 'react'

const StyledTab: FC<{ children: string; isDisabled: boolean }> = ({ children, isDisabled = false }) => (
  <Tab _selected={{ borderColor: 'tealHero' }} _focus={{ borderColor: 'tealHero' }} isDisabled={isDisabled}>
    <Heading as='h3' fontSize='18px'>
      {children}
    </Heading>
  </Tab>
)

const CreatePackagePage: NextPage = () => {
  const [tabIndex, setTabIndex] = useState(0)
  const [packageName, setPackageName] = useState<string>('')

  const router = useRouter()

  const onCreatePackage = (name: string) => {
    setTabIndex(1)
    setPackageName(name)
  }

  useEffect(() => {
    checkSession()
  }, [])

  return (
    <Box w='100%' p='5vh 10vw'>
      <Tabs index={tabIndex} isFitted variant='line'>
        <TabList>
          <StyledTab isDisabled={tabIndex !== 0}>Create a package</StyledTab>
          <StyledTab isDisabled={tabIndex !== 1}>Add a version</StyledTab>
        </TabList>
        <TabPanels>
          <TabPanel>
            <CreatePackageForm onSubmit={onCreatePackage} />
          </TabPanel>
          <TabPanel>
            <CreateFirstVersionForm onSubmit={() => router.push(`/${packageName}`)} packageName={packageName} />
          </TabPanel>
        </TabPanels>
      </Tabs>
    </Box>
  )
}

export default CreatePackagePage
