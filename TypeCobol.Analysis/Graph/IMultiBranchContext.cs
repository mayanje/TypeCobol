﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeCobol.Analysis.Graph
{
    /// <summary>
    /// Multi Branch Context interface for Block that participate to a branching in the Control Flow Graph.
    /// </summary>
    public interface IMultiBranchContext<N, D>
    {
        /// <summary>
        /// The multi branch instruction associated to this context.
        /// </summary>
        N Instruction { get; }
        /// <summary>
        /// Origin block before multi branches
        /// </summary>
        BasicBlock<N, D> OriginBlock { get; }
        /// <summary>
        /// Root Block of the multi branches
        /// </summary>
        BasicBlock<N, D> RootBlock { get; }
        /// <summary>
        /// List of multi branch blocks
        /// </summary>
        IList<BasicBlock<N, D>> Branches { get; }
        /// <summary>
        /// Indices in the CFG SuccessorEdges of all blocks in Branches
        /// </summary>
        IList<int> BranchIndices { get; }
        /// <summary>
        /// Terminals block associated to Multi branch Context if any.
        /// Terminals blocks are block that have as successor NextFlowBlock or
        /// a branching to the beginning of a loop instruction block.
        /// </summary>
        IList<BasicBlock<N, D>> Terminals { get; }
        /// <summary>
        /// Block of the next flow
        /// </summary>
        BasicBlock<N, D> NextFlowBlock { get; }
        /// <summary>
        /// Any Sub context if any, null otherwise.
        /// For instance in COBOL EVALUATE Context has all WHEN and WHENOTHER as sub contexts.
        /// </summary>
        IList<IMultiBranchContext<N, D>> SubContexts { get; }
    }
}
