export interface AddressDetails {
	id: string
	href: string
	vejnavn: string
	husnr: string
	postnr: string
	postnrnavn: string
	adgangsadresseid: string
	x: number
	y: number
	adresseringsvejnavn?: string
	etage?: string | null
	dør?: string | null
	supplerendebynavn?: string | null
	kommunekode?: string
	vejkode?: string
	status?: number
	darstatus?: number
	stormodtagerpostnr?: string | null
	stormodtagerpostnrnavn?: string | null
}

export interface AddressAutocompleteItem {
	tekst: string
	adresse: AddressDetails
}

