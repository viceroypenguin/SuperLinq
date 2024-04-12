// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
	"Naming",
	"CA1711:Identifiers should not have incorrect suffix",
	Justification = "PriorityQueue is the proper name for this Data Structure",
	Scope = "type",
	Target = "~T:SuperLinq.Collections.UpdatablePriorityQueue`2"
)]

[assembly: SuppressMessage(
	"Design",
	"CA1010:Generic interface should also be implemented",
	Justification = "IReadOnlyCollection is preferred over ICollection",
	Scope = "type",
	Target = "~T:SuperLinq.Collections.UpdatablePriorityQueue`2.UnorderedItemsCollection"
)]
